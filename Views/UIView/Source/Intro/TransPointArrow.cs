using UnityEngine;

namespace UIView.Intro
{
    public class TransPointArrow : MonoBehaviour
    {
        public Transform 旋转 = null;
        public UIWidget 箭头 = null;

        //public void OnStartArrow(SceneTransPoint obj)
        //{
        //    StopAllCoroutines();
        //    箭头.gameObject.SetActive(false);
        //    StartCoroutine(DoUpdateArrow(obj));
        //}
        //IEnumerator DoUpdateArrow(SceneTransPoint obj)
        //{
        //    if (null != 箭头)
        //    {
        //        Segment2D dirLine = new Segment2D();
        //        float helfHeight = LocalPlayer.Inst.MoveBoxSize.y * 0.5f;
        //        GameObject lpController = LocalPlayer.Inst.Controller;
        //        Vector3 startPos = Vector3.zero;
        //        Vector3 endPos = obj.transform.localPosition;
        //        Vector3 uiStartPos = Vector3.zero;
        //        Vector3 uiEndPos = Vector3.zero;
        //        Vector3 rightDir = Vector3.right;
        //        Vector3 dir = Vector3.zero;
        //        float dist = 0;
        //        float angle = 0;
        //        Vector3 crossPoint = Vector3.zero;
        //        while (true)
        //        {
        //            yield return 0;
        //            if(null != lpController)
        //            {
        //                startPos = lpController.transform.localPosition;
        //                startPos.y += helfHeight;
        //                uiStartPos = NGUIUtil.U3DWorldPos2NGUIViewPortPoint(startPos);
        //                uiEndPos = NGUIUtil.U3DWorldPos2NGUIViewPortPoint(endPos);
        //                dirLine.ChangeTo2P1(uiStartPos, uiEndPos);
        //                dir = dirLine.Dir;
        //                dist = dirLine.Length;
        //                List<Vector2> crossPts = dirLine.IntersectSque1();
        //                if (null != crossPts && crossPts.Count > 0)
        //                {
        //                    float crossLength = Vector2.Distance(crossPts[0], uiStartPos);
        //                    crossPoint = uiStartPos + dir * crossLength * 0.8f;
        //                }
        //                else
        //                {
        //                    float crossLength = dist;
        //                    crossPoint = uiStartPos + dir * crossLength * 0.8f;
        //                }
        //                transform.position = NGUIUtil.NGUIViewPortPos2NGUIWorldPoint(crossPoint);
        //                //角度=弧度*180.0f/3.14159f
        //                //弧度=角度*3.14159f/180.0f
        //                if (dir.y > 0)
        //                    angle = Mathf.Acos(Vector3.Dot(dir, rightDir)) * Mathf.Rad2Deg;
        //                else
        //                    angle = -Mathf.Acos(Vector3.Dot(rightDir, dir)) * Mathf.Rad2Deg;
        //                旋转.localRotation = Quaternion.Euler(0, 0, angle);
        //                if (!箭头.gameObject.activeSelf)
        //                    箭头.gameObject.SetActive(true);
        //            }
        //        }
        //    }
        //}
    }
}