using System;
using System.Collections.Generic;

public class EventManager
{
	//接口。只负责存在在字典中
	public interface IRegisterations
	{
	
	}
	//多个注册
	public class Registerations<T> : IRegisterations
	{
		//委托本身可以一对多注册
		public Action<T> OnReceives = obj => { };
	}
	
	//事件字典
	private static Dictionary<Type,IRegisterations> mTyperEventDic = new Dictionary<Type,IRegisterations>();
	
	//注册事件
	public static void Register<T>(Action<T> onReceive)
	{
		var type = typeof(T);
		IRegisterations registerations = null;
		if(mTyperEventDic.TryGetValue(type,out registerations ))
		{
			var reg = registerations as Registerations<T>;
			reg.OnReceives += onReceive;
		}
		else
		{
			var reg = new Registerations<T>();
			reg.OnReceives += onReceive;
			mTyperEventDic.Add(type,reg);
		}
	}
	
	//注销事件
	public static void UnRegister<T>(Action<T> onReceive)
	{
		var type = typeof(T);
		IRegisterations registerations = null;
		if(mTyperEventDic.TryGetValue(type,out registerations ))
		{
			var reg = registerations as Registerations<T>;
			reg.OnReceives -= onReceive;
		}
	}
	
	//发送事件
	public static void Send<T>(T t)
	{
		var type = typeof(T);
		IRegisterations registerations = null;
		if(mTyperEventDic.TryGetValue(type,out registerations ))
		{
			var reg = registerations as Registerations<T>;
			reg.OnReceives(t);
		}
	}
}
