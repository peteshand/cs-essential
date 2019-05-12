using System;
using System.Collections.Generic;
using System.Timers;
using notifier;
using signals;
using static Trace;

namespace condition
{
    public class Condition
    {
        private List<ICase> cases = new List<ICase>(); //private ICase[] cases = new ICase[1];
        //public var numCases(get, never):Int;
        public Notifier<bool> active = new Notifier<bool>(true);

        public SignalA onActive;
        public SignalA onInactive;
        public bool value { get; }

        private ICase currentCase;

        public float activeDelay = 0;
        public float inactiveDelay = 0;
        //var timer:Timer;

        public Condition(Action activeCallback, Action inactiveCallback)
        {
            onActive = new SignalA(active, true);
            onInactive = new SignalA(active, false);

            active.Add((value) => {
                if (value) onActive.Dispatch();
                else onInactive.Dispatch();
            });

            onActive.Add(activeCallback);
            onInactive.Add(inactiveCallback);
        }

        public Condition()
        {
            onActive = new SignalA(active, true);
            onInactive = new SignalA(active, false);

            active.Add((value) => {
                if (value) onActive.Dispatch();
                else onInactive.Dispatch();
            });
            trace("123");
        }



        public Condition Add<T>(Notifier<T> notifier, string operation, T targetValue, string subProp = null, bool wildcard = false) 
        {
            /*       
            if (!Operation.valid(operation) ) {
                // In the case that the targetValue is a string and the operation is omitted 
                // then the targetValue will incorrectly be set into the operation
                targetValue = operation;
                operation = EQUAL;
            }
            */

            _add(new Case<T>(notifier, operation, targetValue, subProp, wildcard));
            return this;
        }

        private void _add<T>(Case<T> _case)
        {
            currentCase = _case;
            currentCase.Add(onConditionChange); //currentCase.Add(onConditionChange, 1000);
            cases.Add(currentCase);
            check();
        }

        public bool check(bool forceDispatch = false)
        {
            foreach (var _case in cases)
            {
                _case.check(forceDispatch);
            }

            onConditionChange(active.Value);
            if (forceDispatch == true) active.Dispatch(active.Value);
            return active.Value;
        }

        private void onConditionChange<T>(T value)
        {
            bool _value = checkWithPolicy();
            //float delay = inactiveDelay;
            //if (Value == true) delay = activeDelay;

            //if (timer != null)
            //{
            //    timer.stop();
            //    timer = null;
            //}
            //if (delay == 0) active.Value = Value;
            //else timer = Timer.delay(()-> { active.value = value; }, Math.floor(delay * 1000));

            active.Value = _value;
        }

        private bool checkWithPolicy()
        {
            string bitOperator = BitOperator.AND;
            bool _value = true;
            foreach (var _case in cases)
            {
                bool caseValue = _case.check();
                if (bitOperator == BitOperator.AND) _value = _value && caseValue;
                else if (bitOperator == BitOperator.OR) _value = _value || caseValue;
                else if (bitOperator == BitOperator.XOR) _value = (_value && !caseValue) || (!_value && caseValue);
                bitOperator = _case.bitOperator;
            }
            return _value;
        }

    }

    class Case<T> : Notifier<bool>, ICase
    {
        public Notifier<T> notifier;
        public string operation;
        public string subProp;
        public bool wildcard;
        public string bitOperator { get; set; }
        private bool targetIsFunction;
        //private T testValue { get; set; }
        private T testValue
        {
            get
            {
                return notifier.Value;
            }
        }

        //public var targetValue(get, null):Dynamic;
        private T _targetValue { get; set; }
        //private var _targetFunction:Void -> Dynamic;

        private delegate bool GetValue(T value1, T value2); //private var getValue:Dynamic -> Dynamic -> Bool;
        private GetValue getValue;

        public Case(Notifier<T> notifier, string operation, T _targetValue, string subProp=null, bool wildcard=false) 
        {
            bitOperator = BitOperator.AND;
            this.operation = operation;
            //targetIsFunction = Reflect.isFunction(_targetValue);
            //if (targetIsFunction) _targetFunction = _targetValue;
            //else this._targetValue = _targetValue;
            this._targetValue = _targetValue;

            this.notifier = notifier;
            this.subProp = subProp;

            if (operation == Operation.EQUAL)
            {
                if (wildcard) getValue = wildcardEqualTo;
                else getValue = equalTo;
            }
            else if (operation == Operation.NOT_EQUAL) getValue = notEqualTo;
            else if (operation == Operation.LESS_THAN_OR_EQUAL) getValue = lessThanOrEqualTo;
            else if (operation == Operation.LESS_THAN) getValue = lessThan;
            else if (operation == Operation.GREATER_THAN_OR_EQUAL) getValue = greaterThanOrEqualTo;
            else if (operation == Operation.GREATER_THAN) getValue = greaterThan;

            //super();
            notifier.Add((value) =>
            {
                //Console.WriteLine("value = " + value);
                check();
            });
            check();

            /*
             notifier.add(() -> {
                check();
            }, 1000);
            check();
             */
        }

        public bool check(bool forceDispatch = false)
        {
            //if (targetIsFunction) this.value = _targetFunction();
            //else this.value = getValue(testValue, targetValue);
            //Console.WriteLine("testValue = " + testValue);
            //Console.WriteLine("_targetValue = " + _targetValue);

            //Console.WriteLine(EqualityComparer<T>.Default.Equals(testValue, _targetValue));

            this.Value = getValue(testValue, _targetValue);
            //Console.WriteLine("this.Value = " + this.Value);
            if (forceDispatch) this.Dispatch(Value);
            return Value;
        }

        private bool wildcardEqualTo(T value1, T value2) {
            string str1 = (string)(object)value1;
            string str2 = (string)(object)value2;
            if (str1 == null) str1 = "";
            if (str2 == null) str2 = "";
            return str1.Contains(str2);
        }
        private bool equalTo(T value1, T value2) {
            //Console.WriteLine("equalTo");
            //Console.WriteLine(value1);
            //Console.WriteLine(value2);
            //Console.WriteLine(value1 == value2);
            return EqualityComparer<T>.Default.Equals(value1, value2);
            //return value1 == value2;
        }
        private bool notEqualTo(T value1, T value2) { return !EqualityComparer<T>.Default.Equals(value1, value2); }
        private bool lessThanOrEqualTo(T value1, T value2) { return (float)(object)value1 <= (float)(object)value2; }
        private bool lessThan(T value1, T value2) { return (float)(object)value1 < (float)(object)value2; }
        private bool greaterThanOrEqualTo(T value1, T value2) { return (float)(object)value1 >= (float)(object)value2; }
        private bool greaterThan(T value1, T value2) { return (float)(object)value1 > (float)(object)value2; }

        /*

        function get_targetValue():Dynamic
        {
            if (targetIsFunction) return _targetFunction();
            else return _targetValue;
        }
        


        function get_testValue()
        {
            if (subProp == null) return notifier.value;
            else {
                var split:Array<String> = subProp.split(".");
                if (subProp.indexOf(".") == -1) split = [subProp];
                
                var value:Dynamic = notifier.value;
                while (split.length > 0 && value != null){
                    var prop:String = split.shift();
                    value = Reflect.getProperty(value, prop);
                }
                
                return value;
            }
        }
        


        override function toString():String
        {
            return testValue + " " + operation + " " + targetValue;
            //return "[Case] " + testValue + " " + operation + " " + targetValue + " | " + value + " | " + (testValue == targetValue);
        }

        public function match(notifier:Notifier<Dynamic>, targetValue:Dynamic=null, ?operation:Operation=null, subProp:String=null, wildcard:Bool=false):Bool
        {
            if (this.notifier != notifier) return false;
            if (this.targetValue != targetValue && targetValue != null) return false;
            if (this.operation != operation && operation != null) return false;
            if (this.subProp != subProp && subProp != null) return false;
            if (this.wildcard != wildcard) return false;
            return true;
        }

        public function clone():Case
        {
            var _clone:Case = new Case(notifier, operation, _targetValue, subProp);
            _clone._targetFunction = _targetFunction;
            _clone.targetIsFunction = targetIsFunction;
            return _clone;
        }
        */
    }

    interface ICase
    {
        string bitOperator { get; set; }
        bool check(bool forceDispatch = false);

        void Add(Action<bool> callback);
        //function Add(callback:Void -> Void, ? fireOnce:Bool= false, ? priority:Int = 0, ? fireOnAdd:Null<Bool> = null):Void;
        //function Remove(callback:EitherType<Bool, Void -> Void>=false):Void;
    }

    public class SignalA : Signal0
    {
        private Notifier<bool> active;
        private bool target;

        public SignalA(Notifier<bool> active, bool target)
        {
            this.active = active;
            this.target = target;
        }

        public void Add(Action callback)
        {
            Handlers += callback.Invoke;
            /*
            callbacks.push({
                callback:callback,
                callCount:0,
                fireOnce:fireOnce,
                priority:priority,
                remove:false
            });
            if (priority != 0) priorityUsed = true;
            if (priorityUsed == true) requiresSort = true;
            */
            if (active.Value == target) callback();
        }
    }

}
