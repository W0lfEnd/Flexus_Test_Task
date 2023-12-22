using UnityEngine;


public static class TransformExtensions
{
  public static Vector3 setX( this Vector3 data, float x )
  {
    return new Vector3( x, data.y, data.z );
  }

  public static Vector3 setY( this Vector3 data, float y )
  {
    return new Vector3( data.x, y, data.z );
  }

  public static Vector3 setZ( this Vector3 data, float z )
  {
    return new Vector3( data.x, data.y, z );
  }


  public static Vector2 setX( this Vector2 data, float x )
  {
    return new Vector2( x, data.y );
  }

  public static Vector2 setY( this Vector2 data, float y )
  {
    return new Vector2( data.x, y );
  }

  public static Vector3 setZ( this Vector2 data, float z )
  {
    return new Vector3( data.x, data.y, z );
  }

  public static Vector3 plusX( this Vector3 data, float x )
  {
    return new Vector3( data.x + x, data.y, data.z );
  }

  public static Vector3 plusY( this Vector3 data, float y )
  {
    return new Vector3( data.x, data.y + y, data.z );
  }

  public static Vector3 plusZ( this Vector3 data, float z )
  {
    return new Vector3( data.x, data.y, data.z + z );
  }

  public static Vector3 plusX( this Vector2 data, float x )
  {
    return new Vector3( data.x + x, data.y, 0.0f );
  }

  public static Vector3 plusY( this Vector2 data, float y )
  {
    return new Vector3( data.x, data.y + y, 0.0f );
  }

  public static Vector2 withMin( this Vector2 data, Vector2 min )
  {
    return new Vector2( data.x.withMin( min.x ), data.y.withMin( min.y ) );
  }

  public static Vector3 withMin( this Vector3 data, Vector3 min )
  {
    return new Vector3( data.x.withMin( min.x ), data.y.withMin( min.y ), data.z.withMin( min.z ) );
  }

  public static Vector2 withMax( this Vector2 data, Vector2 max )
  {
    return new Vector3( data.x.withMax( max.x ), data.y.withMax( max.y ) );
  }

  public static Vector3 withMax( this Vector3 data, Vector3 max )
  {
    return new Vector3( data.x.withMax( max.x ), data.y.withMax( max.y ), data.z.withMax( max.z ) );
  }

  public static Vector2 toRange( this Vector2 data, Vector2 min, Vector2 max )
  {
    return data.withMin( min ).withMax( max );
  }

  public static Vector3 toRange( this Vector3 data, Vector3 min, Vector3 max )
  {
    return data.withMin( min ).withMax( max );
  }
}