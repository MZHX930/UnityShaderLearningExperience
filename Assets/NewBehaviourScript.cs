using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public ComputeShader shader;

    public RenderTexture tex1;
    public RenderTexture tex2;

    //public int[] DataParams = new int[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
    IntPair[] intData = new IntPair[5];

    VecMatPair[] data = new VecMatPair[5];

    private void Start()
    {
        RunShader();
    }

    void RunShader()
    {
        int kernelHandle = shader.FindKernel("CSMain");

        tex1 = new RenderTexture(256, 256, 24);
        tex1.enableRandomWrite = true;
        tex1.Create();

        shader.SetTexture(kernelHandle, "Result", tex1);
        shader.Dispatch(kernelHandle, 256 / 16, 256 / 16, 1);

        /*----------------------------------------------*/


        ComputeBuffer buffer = new ComputeBuffer(data.Length, 76);
        buffer.SetData(data);

        int kernel = shader.FindKernel("Multiply");
        shader.SetBuffer(kernel, "dataBuffer", buffer);
        shader.Dispatch(kernel, data.Length, 1, 1);


        /*----------------------------------------------*/
        ComputeBuffer intBuffer = new ComputeBuffer(intData.Length, 4);
        intBuffer.SetData(intData);

        int calIntHandle = shader.FindKernel("CalInt");
        shader.SetBuffer(calIntHandle, "dataInt", intBuffer);
        shader.Dispatch(calIntHandle, intData.Length, 1, 1);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            string content = "";
            foreach (var item in intData)
            {
                content += $"{item.Value}; ";
            }
            Debug.Log(content);
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            string content = "";
            foreach (var item in data)
            {
                content += $"{item.point}; ";
            }
            Debug.Log(content);
        }
    }
}


struct VecMatPair
{
    public Vector3 point;
    public Matrix4x4 matrix;
}

[System.Serializable]
struct IntPair
{
    public int Value;
}
