using MiniProject.Models.Base;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace MiniProject.Repository
{
    internal class Repository<T> where T : BaseEntity
    {
        public void Serialize(string path,List<T> items)
        {


            string jsonTxt = JsonConvert.SerializeObject(items);

            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.Write(jsonTxt);
            } 
        }

        public void Serialize(string path, T items)
        {


            string jsonTxt = JsonConvert.SerializeObject(items);

            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.Write(jsonTxt);
            }
        }

        public List<T> Deserialize(string path)
        {
            string Return = string.Empty;

            using(StreamReader rd = new(path))
            {
                Return = rd.ReadToEnd();
            }

            List<T> list = null;

            if(String.IsNullOrEmpty(Return))
            {
                list = new List<T>();
            }
            else
            {
                list = JsonConvert.DeserializeObject<List<T>>(Return);
            }

            return list;
        }



    }
}
