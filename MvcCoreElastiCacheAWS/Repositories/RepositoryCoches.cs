using MvcCoreElastiCacheAWS.Models;
using System.Xml.Linq;

namespace MvcCoreElastiCacheAWS.Repositories {
    public class RepositoryCoches {
        private XDocument doc;

        public RepositoryCoches() {
            //Para leer recursos incrustados necesitamos le nombre
            //de la libreria (namespace) y el nomrbe del fichero
            string resourceName = "MvcCoreElastiCacheAWS.coches.xml";
            //Para recuperar un recurso se utiliza Stream
            Stream stream = this.GetType().Assembly.GetManifestResourceStream(resourceName);
            this.doc = XDocument.Load(stream);
        }



        public List<Coche> GetCoches() {
            //Vamos a extraer los datos de cada element dentro
            //del documento xml y crear la coleccion directamente
            var consulta = from datos in this.doc.Descendants("coche")
                           select new Coche {
                               IdCoche = int.Parse(datos.Element("idcoche").Value),
                               Marca = datos.Element("marca").Value,
                               Modelo = datos.Element("modelo").Value,
                               Imagen = datos.Element("imagen").Value
                           };
            return consulta.ToList();
        }

        public Coche FindCoche(int idcoche) {
            return this.GetCoches().FirstOrDefault(x => x.IdCoche == idcoche);
        }
    }
}

