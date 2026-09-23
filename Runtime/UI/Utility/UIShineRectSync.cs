using UnityEngine;
using UnityEngine.UI;

// Gắn vào Image dùng shader "Custom/UI/sd_URP_UI_Image" có Image Type = Sliced/Tiled/Filled
// (Simple ngoài atlas thì không cần). Ghi vị trí từng vertex, chuẩn hoá 0..1 theo rect của
// chính Image này, vào UV1 để shine quét thẳng chéo hết item thay vì chạy theo lưới 9-slice.
//
// Không đẩy "_Rect" vào material nữa: vertex UI tới shader ở toạ độ Canvas (đã batch), còn
// RectTransform.rect là local của Image — so 2 hệ đó với nhau là lệch ngay khi Image không nằm
// giữa Canvas. Ghi theo vertex thì mọi Image dùng chung được 1 material và vẫn batch.
[DisallowMultipleComponent]
[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(Graphic))]
public class UIShineRectSync : BaseMeshEffect
{
    protected override void OnEnable()
    {
        base.OnEnable();
        EnableTexCoord1();
    }

    protected override void OnTransformParentChanged()
    {
        base.OnTransformParentChanged();
        EnableTexCoord1();
    }

    // Canvas mặc định không gửi UV1 xuống shader.
    private void EnableTexCoord1()
    {
        var canvas = graphic != null ? graphic.canvas : null;
        if (canvas == null) return;

        canvas.additionalShaderChannels |= AdditionalCanvasShaderChannels.TexCoord1;
        canvas.rootCanvas.additionalShaderChannels |= AdditionalCanvasShaderChannels.TexCoord1;
    }

    public override void ModifyMesh(VertexHelper vh)
    {
        if (!IsActive()) return;

        Rect rect = graphic.rectTransform.rect;
        float width = Mathf.Max(rect.width, 0.0001f);
        float height = Mathf.Max(rect.height, 0.0001f);

        UIVertex vertex = default;
        for (int i = 0; i < vh.currentVertCount; i++)
        {
            vh.PopulateUIVertex(ref vertex, i);

            // z = 1 để shader biết UV1 này có dữ liệu thật, không phải 0 mặc định.
            Vector3 pos = vertex.position;
            vertex.uv1 = new Vector4((pos.x - rect.xMin) / width, (pos.y - rect.yMin) / height, 1f, 0f);

            vh.SetUIVertex(vertex, i);
        }
    }
}
