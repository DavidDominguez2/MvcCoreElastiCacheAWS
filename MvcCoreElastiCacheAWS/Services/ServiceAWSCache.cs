using MvcCoreElastiCacheAWS.Helpers;
using MvcCoreElastiCacheAWS.Models;
using MvcCoreElastiCacheAWS.Repositories;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace MvcCoreElastiCacheAWS.Services {
    public class ServiceAWSCache {
        private IDatabase cache;
        private RepositoryCoches repo;

        public ServiceAWSCache(RepositoryCoches repo) {
            this.cache = HelperCacheRedis.Connection.GetDatabase();
            this.repo = repo;
        }

        public async Task<List<Coche>> GetCochesFavoritosAsync() {
            return this.repo.GetCoches();
            //string jsonCoches = await this.cache.StringGetAsync("cochesfavoritos");
            //if (jsonCoches == null) {
            //    return null;
            //} else {
            //    List<Coche> cars = JsonConvert.DeserializeObject<List<Coche>>(jsonCoches);
            //    return cars;
            //}
        }


        public async Task AddCocheAsync(Coche coche) {
            List<Coche> cars = await this.GetCochesFavoritosAsync();
            if (cars == null) {
                cars = new List<Coche>();
            }
            cars.Add(coche);
            string jsonCoches = JsonConvert.SerializeObject(cars);
            await this.cache.StringSetAsync("cochesfavoritos", jsonCoches, TimeSpan.FromMinutes(30));
        }

        public async Task DeleteCocheFavoritoAsync(int idcoche) {
            List<Coche> coches = await this.GetCochesFavoritosAsync();
            if (coches != null) {
                Coche carEliminar = coches.FirstOrDefault(x => x.IdCoche == idcoche);
                coches.Remove(carEliminar);
                if (coches.Count == 0) {
                    await this.cache.KeyDeleteAsync("cochesfavoritos");
                } else {
                    string jsonCoches = JsonConvert.SerializeObject(coches);
                    await this.cache.StringSetAsync("cochesfavoritos", jsonCoches, TimeSpan.FromMinutes(30));
                }
            }
        }
    }
}
