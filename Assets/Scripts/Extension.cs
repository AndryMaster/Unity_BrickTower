using UnityEngine;

public static class ExtensionMethods
{
    // public static Vector3 Round(this Vector3 vector3, float delta)
    // {
    //     return new Vector3(
    //         Mathf.Round(vector3.x / delta) * delta,
    //         Mathf.Round(vector3.y / delta) * delta,
    //         Mathf.Round(vector3.z / delta) * delta);
    // } 
    
    public static Vector3 Abs(this Vector3 vector3)
    {
        return new Vector3(
            Mathf.Abs(vector3.x),
            Mathf.Abs(vector3.y),
            Mathf.Abs(vector3.z));
    }
    
}
