using UnityEngine;

public static class GameObjectCreator
{ 
    public static GameObject New(string name = null, string catalogue = "no_catalogue")
    {
        GameObject ret = null;
        if (string.IsNullOrEmpty(name))
            ret = new GameObject();
        else
            ret = new GameObject(name);
        return ret;
    }
}
