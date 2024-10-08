using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPooling : MonoBehaviour
{
    public Bullet Pobj; // Bullet Ÿ������ ����

    void Start()
    {
        // �ʱ� ������Ʈ Ǯ�� 30�� �̸� ����
        ObjectPoolManager<Bullet>.Instance.Pool(Pobj, 30);
        StartCoroutine(poolTest());
    }

    IEnumerator poolTest()
    {
        while (true)
        {
            // 1�ʸ��� Ǯ���� ������Ʈ ������
            yield return new WaitForSeconds(1f);
            Bullet obj = ObjectPoolManager<Bullet>.Instance.PopPool(Pobj, transform.position);

            // ������Ʈ ���: ���⿡ ��� ������ �־��൵ �� (��: �̵�, ���� ��)

            // 1�� �� ������Ʈ �ٽ� Ǯ�� ��ȯ
            yield return new WaitForSeconds(1f);
            ObjectPoolManager<Bullet>.Instance.ReturnToPool(obj);
        }
    }
}
