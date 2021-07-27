using UnityEngine;

public static class UIUtils
{
    public static Vector2 GetScreenOffset()
    {
        float ratio = (float)Screen.height / Screen.width;

        if (ratio >= 1.9f)
        {
            // S: пока подразумаваем, что на девайсах с таким аспектом есть моноброви
            return new Vector2(0, -120);
        }
        else
        {
            return Vector2.zero;
        }
    }
}
