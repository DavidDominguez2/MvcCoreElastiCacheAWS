using Microsoft.Extensions.Caching.Distributed;
using MvcCoreElastiCacheAWS.Helpers;
using MvcCoreElastiCacheAWS.Models;
using MvcCoreElastiCacheAWS.Repositories;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace MvcCoreElastiCacheAWS.Services {
    public class ServiceAWSCache {
        private readonly IDistributedCache cache;
        private RepositoryCoches repo;

        public ServiceAWSCache(RepositoryCoches repo, IDistributedCache cache) {
            this.cache = cache;
            this.repo = repo;
        }

        public async Task<List<Coche>> GetCochesFavoritosAsync() {
            string jsonCoches = await this.cache.GetStringAsync("cochesfavoritos");
            if (jsonCoches == null) {
                return null;
            } else {
                List<Coche> cars = JsonConvert.DeserializeObject<List<Coche>>(jsonCoches);
                return cars;
            }
        }


        public async Task AddCocheAsync(Coche coche) {
            List<Coche> cars = await this.GetCochesFavoritosAsync();
            if (cars == null) {
                cars = new List<Coche>();
            }
            cars.Add(coche);
            string jsonCoches = JsonConvert.SerializeObject(cars);
            var options = new DistributedCacheEntryOptions(); // create options object
            options.SetSlidingExpiration(TimeSpan.FromMinutes(1)); // 1 minute sliding expiration
            await this.cache.SetStringAsync("cochesfavoritos", jsonCoches, options);
        }

        public async Task DeleteCocheFavoritoAsync(int idcoche) {
            List<Coche> coches = await this.GetCochesFavoritosAsync();
            if (coches != null) {
                Coche carEliminar = coches.FirstOrDefault(x => x.IdCoche == idcoche);
                coches.Remove(carEliminar);
                if (coches.Count == 0) {
                    await this.cache.RemoveAsync("cochesfavoritos");
                } else {
                    string jsonCoches = JsonConvert.SerializeObject(coches);
                    await this.cache.SetStringAsync("cochesfavoritos", jsonCoches);
                }
            }
        }
    }
}
