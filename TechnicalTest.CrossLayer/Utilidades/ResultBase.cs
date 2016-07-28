using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TechnicalTest.CrossLayer
{
    /// <summary>
    /// Lista T Resultado Base
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [DataContract]
    public class ResultBase<T>
    {
        /// <summary>
        /// Exitoso
        /// </summary>
        [DataMember]
        public Boolean Success;
        
        /// <summary>
        /// Lista de Datos
        /// </summary>
        [DataMember]
        public List<T> DataList;

        /// <summary>
        /// Array de Datos
        /// </summary>
        [DataMember]
        public T[] DataArray;

        /// <summary>
        /// Dato Sencillo
        /// </summary>
        [DataMember]
        public T DataSingle;

        /// <summary>
        /// Mensaje Resultado
        /// </summary>
        [DataMember]
        public String Message;

        /// <summary>
        /// Resultado Entero
        /// </summary>
        [DataMember]
        public int Results;
    }
}
