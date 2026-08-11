using System.Collections;
using UnityEngine;

public class FaceController : MonoBehaviour
{
    [Header("Face Mesh")]
    public SkinnedMeshRenderer face;

    // =========================
    // BLINK
    // =========================
    int blinkIndex;
    float blinkValue;
    float blinkTarget;

    [Header("Blink Settings")]
    public float blinkMinTime = 2f;
    public float blinkMaxTime = 5f;
    public float blinkSpeed = 1f;

    bool blockBlink = false;

    // =========================
    // EXPRESSIONS (ALL 100 OR 0)
    // =========================
    int eyesHappy;
    int eyesCrying;
    int eyesWhite;
    int eyesFurious;
    int eyesRelax;

    int mouthSmile;
    int mouthSad;
    int mouthShock;
    int mouthNeko;

    int browAngry;
    int browWorried;
    int browConfident;

    int blush;
    int sweat;

    void Start()
    {
        // Eyes
        blinkIndex = face.sharedMesh.GetBlendShapeIndex("Eyes_Blink");

        eyesHappy = face.sharedMesh.GetBlendShapeIndex("Eyes_Happy");
        eyesCrying = face.sharedMesh.GetBlendShapeIndex("Eyes_Crying");
        eyesWhite = face.sharedMesh.GetBlendShapeIndex("Eyes_White");
        eyesFurious = face.sharedMesh.GetBlendShapeIndex("Eyes_Furious");
        eyesRelax = face.sharedMesh.GetBlendShapeIndex("Eyes_Relax");

        // Mouth
        mouthSmile = face.sharedMesh.GetBlendShapeIndex("Mouth_Smile");
        mouthSad = face.sharedMesh.GetBlendShapeIndex("Mouth_Sad");
        mouthShock = face.sharedMesh.GetBlendShapeIndex("Mouth_Shock");
        mouthNeko = face.sharedMesh.GetBlendShapeIndex("Mouth_Neko");

        // Brows
        browAngry = face.sharedMesh.GetBlendShapeIndex("Brow_Angry");
        browWorried = face.sharedMesh.GetBlendShapeIndex("Brow_Worried");
        browConfident = face.sharedMesh.GetBlendShapeIndex("Brow_Confident");

        // FX
        blush = face.sharedMesh.GetBlendShapeIndex("Blush");
        sweat = face.sharedMesh.GetBlendShapeIndex("Sweat");

        StartCoroutine(BlinkLoop());
        StartCoroutine(ExpressionDemoLoop());
    }

    void Update()
    {
        // 👀 BLINK SMOOTH ONLY
        blinkValue = Mathf.Lerp(blinkValue, blinkTarget, Time.deltaTime * 20f * blinkSpeed);
        face.SetBlendShapeWeight(blinkIndex, blinkValue);
    }

    // =========================
    // 👀 BLINK LOOP
    // =========================
    IEnumerator BlinkLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(blinkMinTime, blinkMaxTime));

            if (blockBlink)
                continue;

            blinkTarget = 100f;
            yield return new WaitForSeconds(0.08f);
            blinkTarget = 0f;
        }
    }

    // =========================
    // 🎭 DEMO EXPRESSIONS
    // =========================
    IEnumerator ExpressionDemoLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(2f, 5f));

            ResetFace();

            int r = Random.Range(0, 6);

            switch (r)
            {
                case 0: // HAPPY
                    face.SetBlendShapeWeight(eyesHappy, 100f);
                    face.SetBlendShapeWeight(mouthSmile, 100f);
                    face.SetBlendShapeWeight(blush, 100f);
                    break;

                case 1: // SAD
                    face.SetBlendShapeWeight(eyesCrying, 100f);
                    face.SetBlendShapeWeight(mouthSad, 100f);
                    break;

                case 2: // ANGRY
                    face.SetBlendShapeWeight(eyesFurious, 100f);
                    face.SetBlendShapeWeight(browAngry, 100f);
                    face.SetBlendShapeWeight(sweat, 100f);
                    break;

                case 3: // SHOCK (blocks blink)
                    face.SetBlendShapeWeight(eyesWhite, 100f);
                    face.SetBlendShapeWeight(mouthShock, 100f);

                    StartCoroutine(ShockBlockBlink());
                    break;

                case 4: // NEKO CUTE
                    face.SetBlendShapeWeight(mouthNeko, 100f);
                    face.SetBlendShapeWeight(blush, 100f);
                    face.SetBlendShapeWeight(eyesRelax, 100f);
                    break;

                case 5: // CONFIDENT
                    face.SetBlendShapeWeight(eyesHappy, 100f);
                    face.SetBlendShapeWeight(browConfident, 100f);
                    break;
            }
        }
    }

    // =========================
    // 😮 SHOCK CONTROL
    // =========================
    IEnumerator ShockBlockBlink()
    {
        blockBlink = true;

        yield return new WaitForSeconds(1.5f);

        blockBlink = false;
    }

    // =========================
    // 🧼 RESET (ALL TO 0)
    // =========================
    void ResetFace()
    {
        for (int i = 0; i < face.sharedMesh.blendShapeCount; i++)
        {
            face.SetBlendShapeWeight(i, 0f);
        }
    }
}