## NOTE: This page has moved to [IRedisTransaction on GitHub](https://github.com/ServiceStack/ServiceStack.Redis/wiki/IRedisTransaction). Please update your links. ##



&lt;hr/&gt;


<br />

The [RedisTransactions](RedisTransactions.md) interface implemented by [ServiceStack's C# Redis Client](ServiceStackRedis.md).

# Introduction #

Redis transaction interface provides useful overloads that let you Queue-up any [IRedisClient](IRedisClient.md) operation within a single transaction. The API provides support for a callback so you also have access to any return values returned as part of the transaction as well.

# Usage #

Below is a simple example showing how to access, use and commit the transaction.
```
int callbackResult;
using (var trans = redis.CreateTransaction())
{
  trans.QueueCommand(r => r.Increment("key"));  
  trans.QueueCommand(r => r.Increment("key"), i => callbackResult = i);  

  trans.Commit();
}
```

For a strongly-typed transaction that operates on complex POCO types see [IRedisTypedTransaction&lt;T&gt;](IRedisTypedTransaction.md).

# Details #

```
public interface IRedisTransaction 
	: IDisposable
{
	void QueueCommand(Action<IRedisClient> command);
	void QueueCommand(Action<IRedisClient> command, Action onSuccessCallback);
	void QueueCommand(Action<IRedisClient> command, Action onSuccessCallback, Action<Exception> onErrorCallback);

	void QueueCommand(Func<IRedisClient, int> command);
	void QueueCommand(Func<IRedisClient, int> command, Action<int> onSuccessCallback);
	void QueueCommand(Func<IRedisClient, int> command, Action<int> onSuccessCallback, Action<Exception> onErrorCallback);

	void QueueCommand(Func<IRedisClient, bool> command);
	void QueueCommand(Func<IRedisClient, bool> command, Action<bool> onSuccessCallback);
	void QueueCommand(Func<IRedisClient, bool> command, Action<bool> onSuccessCallback, Action<Exception> onErrorCallback);

	void QueueCommand(Func<IRedisClient, double> command);
	void QueueCommand(Func<IRedisClient, double> command, Action<double> onSuccessCallback);
	void QueueCommand(Func<IRedisClient, double> command, Action<double> onSuccessCallback, Action<Exception> onErrorCallback);

	void QueueCommand(Func<IRedisClient, byte[]> command);
	void QueueCommand(Func<IRedisClient, byte[]> command, Action<byte[]> onSuccessCallback);
	void QueueCommand(Func<IRedisClient, byte[]> command, Action<byte[]> onSuccessCallback, Action<Exception> onErrorCallback);

	void QueueCommand(Func<IRedisClient, string> command);
	void QueueCommand(Func<IRedisClient, string> command, Action<string> onSuccessCallback);
	void QueueCommand(Func<IRedisClient, string> command, Action<string> onSuccessCallback, Action<Exception> onErrorCallback);

	void QueueCommand(Func<IRedisClient, List<string>> command);
	void QueueCommand(Func<IRedisClient, List<string>> command, Action<List<string>> onSuccessCallback);
	void QueueCommand(Func<IRedisClient, List<string>> command, Action<List<string>> onSuccessCallback, Action<Exception> onErrorCallback);

	void Commit();
	void Rollback();
}
```