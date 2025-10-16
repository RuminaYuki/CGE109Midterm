using Gamekit3D.GameCommands;
using UnityEngine;

public class SimpleTranslatorController : MonoBehaviour
{
    [SerializeField] private SimpleTransformer SimpleTransformerScript1;
    [SerializeField] private SimpleTransformer SimpleTransformerScript2;
    public bool active;

    public void activate()
    {
        if (SimpleTransformerScript1 != null)
        {
            SimpleTransformerScript1.activate = true;
            active = true;
        }
        if (SimpleTransformerScript2 != null)
        {
            SimpleTransformerScript2.activate = true;
        }
        StartCoroutine(DelayActive());

    }

    private System.Collections.IEnumerator DelayActive()
    {
        yield return new WaitForSeconds(SimpleTransformerScript1.duration * 2f);
        if (SimpleTransformerScript1 != null)
        {
            SimpleTransformerScript1.activate = false;
            active = false;
        }
        if (SimpleTransformerScript2 != null)
        {
            SimpleTransformerScript2.activate = false;
        }
        
    }


}
