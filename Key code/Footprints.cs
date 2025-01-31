using UnityEngine;

public class Footprints : MonoBehaviour
{
    public GameObject footprintsPrefab; // 脚印图片的预制体
    public int numberOfFootprints = 5; // 脚印图片的数量
    public float spawnInterval = 1.0f; // 生成脚印图片的间隔时间
    public float moveSpeed = 1.0f; // 脚印图片的移动速度
    public float resetDistance = 10.0f; // 重置位置的距离

    public Vector3 initialPosition; // 第一张脚印图片的初始位置
    public Quaternion initialRotation; // 第一张脚印图片的初始方向

    private GameObject[] footprints; // 存储脚印图片的数组
    private float lastSpawnTime; // 上一次生成脚印图片的时间

    void Start()
    {
        // 初始化脚印图片数组
        footprints = new GameObject[numberOfFootprints];

        // 生成第一张脚印图片
        footprints[0] = Instantiate(footprintsPrefab, initialPosition, initialRotation);

        // 初始化上一次生成脚印图片的时间
        lastSpawnTime = Time.time;
    }

    void Update()
    {
        // 生成脚印图片
        SpawnFootprints();

        // 移动脚印图片
        MoveFootprints();
    }

    void SpawnFootprints()
    {
        // 如果距离上一次生成脚印图片的时间超过了间隔时间，则生成一个新的脚印图片
        if (Time.time - lastSpawnTime > spawnInterval)
        {
            // 生成一个新的脚印图片
            GameObject newFootprint = Instantiate(footprintsPrefab, initialPosition, initialRotation);

            // 销毁最早生成的脚印图片
            Destroy(footprints[0]);

            // 将新生成的脚印图片添加到数组中
            for (int i = 0; i < numberOfFootprints - 1; i++)
            {
                footprints[i] = footprints[i + 1];
            }
            footprints[numberOfFootprints - 1] = newFootprint;

            // 更新上一次生成脚印图片的时间
            lastSpawnTime = Time.time;
        }
    }

    void MoveFootprints()
    {
        // 将每张脚印图片向前移动
        for (int i = 0; i < numberOfFootprints; i++)
        {
            if (footprints[i] != null)
            {
                footprints[i].transform.Translate(-Vector3.forward * moveSpeed * Time.deltaTime);

                // 如果脚印图片移出屏幕外，则重置位置
                if (footprints[i].transform.position.z > resetDistance)
                {
                    Vector3 currentPosition = footprints[i].transform.position;
                    currentPosition.z -= resetDistance;
                    footprints[i].transform.position = currentPosition;
                }
            }
        }
    }
}
