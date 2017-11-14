using UnityEngine;


[ExecuteInEditMode]
public class RadialBlur : MonoBehaviour
{
    /// <summary>
    /// Shader "Hidden/RadialBlur"
    /// </summary>
    [SerializeField]
    private Material matRadial;

    /// <summary>
    /// Temporary RT variable.
    /// </summary>
    [SerializeField]
    private RenderTexture rtRadial;

    [Range(0, 3)]
    [SerializeField]
    private int downsample = 1;

    /// <summary>
    /// Blur parameters
    /// x = strength; y = width; z = screenPosX, w = screenPosY
    /// </summary>
    public Vector4 BlurStrengthWidthPosition = new Vector4(10f, 0.25f, 0.5f, 0.5f);


    private const string BLUR_STRENGTH_WIDTH_POSITION = "_BlurStrengthWidthPosition";




    private void OnEnable()
    {
        if (matRadial == null)
        {
            matRadial = new Material(Shader.Find("Hidden/RadialBlur"));
        }
        rtRadial = GetRenderTexture();
    }

    private void OnDisable()
    {
        RenderTexture.ReleaseTemporary(rtRadial);
        rtRadial = null;
    }

    void OnRenderImage(RenderTexture source, RenderTexture dest)
    {
#if UNITY_EDITOR
        RenderTexture.ReleaseTemporary(rtRadial);
        rtRadial = GetRenderTexture();
#endif

        matRadial.SetVector(BLUR_STRENGTH_WIDTH_POSITION, BlurStrengthWidthPosition);
        Graphics.Blit(source, rtRadial, matRadial);
        Graphics.Blit(rtRadial, dest);

    }


    RenderTexture GetRenderTexture()
    {
        RenderTexture rt = RenderTexture.GetTemporary(Screen.width >> downsample, Screen.height >> downsample, 0);
        rt.antiAliasing = 1;
        rt.filterMode = FilterMode.Bilinear;
        rt.anisoLevel = 0;
        return rt;
    }
    
}
