using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPooling : MonoBehaviour
{
    public Bullet Pobj; // Bullet 타입으로 변경

    void Start()
    {
        // 초기 오브젝트 풀에 30개 미리 생성
        ObjectPoolManager<Bullet>.Instance.Pool(Pobj, 30);
        StartCoroutine(poolTest());
    }

    IEnumerator poolTest()
    {
        while (true)
        {
            // 1초마다 풀에서 오브젝트 꺼내기
            yield return new WaitForSeconds(1f);
            Bullet obj = ObjectPoolManager<Bullet>.Instance.PopPool(Pobj, transform.position);

            // 오브젝트 사용: 여기에 사용 로직을 넣어줘도 됨 (예: 이동, 공격 등)

            // 1초 뒤 오브젝트 다시 풀에 반환
            yield return new WaitForSeconds(1f);
            ObjectPoolManager<Bullet>.Instance.ReturnToPool(obj);
        }
    }
}
