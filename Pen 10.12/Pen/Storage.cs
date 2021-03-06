using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace Pen
{
   
        public class Storage<T> : System.Collections.IEnumerator
        {
            public Storage()
            {
                _objs = new List<T>();
                _pos = -1;
            }
            T _currentobj = default(T);
            protected List<T> _objs;
            int _pos;
            public object Current { get { return _currentobj; } }

            public void Dispose()
            {
                _currentobj = default(T);
                _objs.Clear();
                _pos = -1;
            }
            public int AddAObj(T obj)
            {
                try
                {
                    _objs.Add(obj);
                }
                catch (Exception Ex)
                {
                    Console.WriteLine(Ex.Message);
                    Console.WriteLine("Add Obj ={ 0}", obj);
                }
                return _objs.Count - 1;
            }
            public void RemoveObj(T obj)
            {
                try
                {
                    _objs.Remove(obj);
                }
                catch (Exception Ex)
                {
                    Console.WriteLine(Ex.Message);
                    Console.WriteLine("Remove Obj={0}", obj);
                }
            }
            public void RemoveObj(int index)
            {
                _objs.RemoveAt(index);
            }
            public bool MoveNext()
            {
                if (_pos < _objs.Count - 1)
                {
                    _pos++;
                    _currentobj = _objs[_pos];
                    return true;
                }
                else
                {
                    _currentobj = _objs[_pos];
                    return false;
                }
            }
            public void Reset()
            {
                _currentobj = default(T);
                _pos = -1;
            }
            public int GetCount() { return _objs.Count; }
            public bool IsContains(T selObj)
            {
                return _objs.Contains(selObj);
            }
            public IEnumerator<T> GetEnumerator()
            {
                return _objs.GetEnumerator();
            }
            public T this[int index]
            {
                get
                {
                    if (index >= 0 && index < _objs.Count) return (T)_objs[index];
                    else throw new Exception("out of range");
                }
                set { if (index == _objs.Count) this.AddAObj(value); else _objs[index] = value; }
            }
            public IEnumerable<T> GetOnlyType<T>()
            {
                for(int i = 0; i < _objs.Count; i++)
                {
                    if(_objs is T)
                    {
                        yield return (T)(object)_objs[i];
                    }
                }
            }
        }
    }

