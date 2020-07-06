using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DC.IDE.UI.Util
{
    public class SingletonProvider<T> where T : new()

    {

        SingletonProvider() { }

        public static T Instance

        {

            get { return SingletonCreator.instance; }

        }

        class SingletonCreator

        {

            static SingletonCreator() { }

            internal static readonly T instance = new T();

        }

    }

}