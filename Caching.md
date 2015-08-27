## NOTE: This page has now moved to [Caching on GitHub](https://github.com/ServiceStack/ServiceStack/wiki/Caching). Please update your links. ##



&lt;hr/&gt;


<br />

A myriad of different, pluggable options are included in Service Stack for the most popular cache providers.

# Introduction #

As caching is an essential technology in the development of high-performance web services, Service Stack has a number of different caching options available that each share the same
[common client interface](http://code.google.com/p/servicestack/source/browse/trunk/Common/ServiceStack.Interfaces/ServiceStack.CacheAccess/ICacheClient.cs)
for the following cache providers:

# Cache Providers #

  * [Memcached](http://code.google.com/p/servicestack/source/browse/#svn/trunk/Common/ServiceStack.Common/ServiceStack.CacheAccess.Memcached) - The tried and tested most widely used cache provider.
  * [Redis](http://code.google.com/p/servicestack/source/browse/trunk/Common/ServiceStack.Redis/ServiceStack.Redis/RedisClient.ICacheClient.cs) - A very fast key-value store that has  non-volatile persistent storage and support for rich data structures such as lists and sets.
  * [In Memory Cache](http://code.google.com/p/servicestack/source/browse/trunk/Common/ServiceStack.Common/ServiceStack.CacheAccess.Providers/MemoryCacheClient.cs) - Useful for single host web services and enabling unit tests to run without needing access to a cache server.
  * [FileAndCacheTextManager](http://code.google.com/p/servicestack/source/browse/trunk/Common/ServiceStack.Common/ServiceStack.CacheAccess.Providers/FileAndCacheTextManager.cs) - A two-tiered cache provider that caches using one of the above cache clients as well as a compressed XML or JSON serialized backup cache on the file system.

## ICacheClient the Common Interface ##

```

//A common interface implementation that is implemeneted by most cache providers

public interface ICacheClient 
	: IDisposable
{
	//Removes the specified item from the cache.
	bool Remove(string key);
	
	//Removes the cache for all the keys provided.
	void RemoveAll(IEnumerable<string> keys);

	//Retrieves the specified item from the cache.
	T Get<T>(string key);

	//Increments the value of the specified key by the given amount. 
	//The operation is atomic and happens on the server.
	//A non existent value at key starts at 0
	long Increment(string key, uint amount);

	//Increments the value of the specified key by the given amount. 
	//The operation is atomic and happens on the server.
	//A non existent value at key starts at 0
	long Decrement(string key, uint amount);
	
	//Adds a new item into the cache at the specified cache key only if the cache is empty.
	bool Add<T>(string key, T value);

	//Sets an item into the cache at the cache key specified regardless if it already exists or not.
	bool Set<T>(string key, T value);

	
	//Replaces the item at the cachekey specified only if an items exists at the location already. 
	bool Replace<T>(string key, T value);

	bool Add<T>(string key, T value, DateTime expiresAt);
	bool Set<T>(string key, T value, DateTime expiresAt);
	bool Replace<T>(string key, T value, DateTime expiresAt);

	bool Add<T>(string key, T value, TimeSpan expiresIn);
	bool Set<T>(string key, T value, TimeSpan expiresIn);
	bool Replace<T>(string key, T value, TimeSpan expiresIn);

	
	//Invalidates all data on the cache.
	void FlushAll();

	//Retrieves multiple items from the cache. 
	//The default value of T is set for all keys that do not exist.
	IDictionary<string, T> GetAll<T>(IEnumerable<string> keys);

	
	//Sets multiple items to the cache. 
	void SetAll<T>(IDictionary<string, T> values);
}
```