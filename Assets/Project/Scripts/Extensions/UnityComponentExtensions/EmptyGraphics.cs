using UnityEngine.UI;

namespace EmojiChat.UI
{
    public class EmptyGraphics : MaskableGraphic
    {
        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
            return;
        }
    }
}