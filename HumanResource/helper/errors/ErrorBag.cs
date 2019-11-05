using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace hr.helper.errors {
    public class ErrorBag : Exception, IDisposable {
        private IDictionary<string, IList<string>> errors;
        private string paramName;
        private object subj;
        private IDictionary<string, IList<string>> _Errors {
            get {
                return this.errors ?? (this.errors = new Dictionary<string, IList<string>>());
            }
        }  

        public ErrorBag(string message = "Error Bag") : base(message) { }

        public ReadOnlyDictionary<string, IList<string>> Errors {
            get {
                return new ReadOnlyDictionary<string, IList<string>>(this.errors);
            }
        }

        public int Count {
            get { 
                return this._Errors.Count;
            }
        }

        public void raiseOnError() {
            this.subj = null;

            if(this.Count > 0)
                throw this;
        }

        public void Dispose()
        {
            this.paramName = null;
            this.subj = null;

            if(this.Count > 0) {
                throw this;
            } else {
                this.errors = null;
            }
        }

        // Validations ...

        public ErrorBag Required(string paramName, object value) {
            if(value == null) {
                this.Add(paramName, "required");
            }
                
            this.paramName = paramName;
            this.subj = value;

            return this;
        }

        public ErrorBag Optional(string paramName, object value) {
            if(value != null) {
                if(!IsNumber(value)) {
                    var s = value as string;
                    if(string.IsNullOrEmpty(s) || string.IsNullOrWhiteSpace(s)) {
                        return this;
                    }
                }
                this.paramName = paramName;
                this.subj = value;
            }

            return this;
        }

        public ErrorBag Min(int n) {
            if(this.subj is string){
                var x = this.subj as string;
                if(x.Length < n)
                    this.Add(this.paramName, $"minimum of {n} characters");
            }
            else if(IsNumber(this.subj)) {
                if((int) this.subj < n)
                    this.Add(this.paramName, $"minimum of {n} characters");
            }
            return this;
        }

        public ErrorBag Max(int n) {
            if(this.subj is string){
                var x = this.subj as string;
                if(x.Length > n)
                    this.Add(this.paramName, $"maximum of {n} characters");
            }
            else if(IsNumber(this.subj)) {
                if((int) this.subj > n)
                    this.Add(this.paramName, $"maximum of {n} characters");
            }
            return this;
        }

        public ErrorBag Format(string format) {
            if(!Regex.IsMatch((string) this.subj, format)) {
                this.Add(this.paramName, "invalid format");
            }
            return this;
        }

        public ErrorBag Alpha() {
            this.Format(@"^[a-zA-Z]+$");
            return this;
        }

        public ErrorBag AlphaSpaces() {
            this.Format(@"^[a-zA-Z\s]*[a-zA-Z]$");
            return this;
        }

        public ErrorBag AlphaNum() {
            this.Format(@"^[a-zA-Z0-9\.\-]+$");
            return this;
        }

        private bool IsNumber(object value) {
            return value is sbyte
                || value is byte
                || value is short
                || value is ushort
                || value is int
                || value is uint
                || value is long
                || value is ulong
                || value is float
                || value is double
                || value is decimal;
        }

        // public void Add(ErrorMessage message) {
        //     if(this.messages == null) this.messages = new List<ErrorMessage>();
        //     this.messages.Add(message);
        // }

        public void Add(string paramName, string message) {
            if(!this._Errors.Keys.Contains(paramName))
                this._Errors.Add(paramName, new List<string>());
            this._Errors[paramName].Add(message);
        }
    }
}