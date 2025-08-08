using System;
using System.IO;
using Spine.Unity;
using UnityEngine;

// Token: 0x02000583 RID: 1411
public class EffectInfo
{
	// Token: 0x06003097 RID: 12439 RVA: 0x0014BEF0 File Offset: 0x0014A0F0
	public static EffectInvoker MakeEffect(EffectInfo info, MovableObjectNode mov)
	{
		EffectInvoker result;
		try
		{
			string[] array = info.effectSrc.Split(new char[]
			{
				'/'
			});
			if (array[0].ToLower() == "custom" && Add_On.instance.EffectList.ContainsKey(array[1]))
			{
				GameObject gameObject = SkeletonAnimation.NewSkeletonAnimationGameObject(Add_On.instance.EffectList[array[1]]).gameObject;
				gameObject.AddComponent<CustomEffect>();
				gameObject.transform.SetParent(EffectLayer.currentLayer.transform);
				gameObject.name = string.Format("{0}[owner:{1}]", gameObject.name, mov.GetUnit().GetUnitName());
				Vector3 vector = gameObject.transform.position;
				Vector3 a = info.relativePosition;
				Vector3 localScale = gameObject.transform.localScale;
				if (mov.GetDirection() == UnitDirection.LEFT)
				{
					localScale.x *= -1f;
					a.x *= -1f;
				}
				a *= mov.currentScale;
				vector += a + mov.GetCurrentViewPosition();
				gameObject.transform.position = vector;
				gameObject.transform.rotation = Quaternion.Euler(0f, 0f, info.rotation);
				gameObject.transform.localScale = localScale;
				gameObject.SetActive(true);
				result = null;
			}
			else
			{
				EffectInvoker effectInvoker = EffectInvoker.Invoker("DamageInfo/" + info.effectSrc, mov, info.lifetime, info.unscaled);
				Vector3 vector2 = effectInvoker.transform.position;
				Vector3 vector3 = info.relativePosition;
				Vector3 localScale2 = effectInvoker.transform.localScale;
				if (mov.GetDirection() == UnitDirection.LEFT)
				{
					localScale2.x *= -1f;
					vector3.x *= -1f;
				}
				effectInvoker.Dettach();
				vector3 *= mov.currentScale;
				vector2 += vector3;
				effectInvoker.transform.position = vector2;
				effectInvoker.transform.rotation = Quaternion.Euler(0f, 0f, info.rotation);
				effectInvoker.transform.localScale = localScale2;
				result = effectInvoker;
			}
		}
		catch (Exception ex)
		{
			File.WriteAllText(Application.dataPath + "/BaseMods/EFerror.txt", ex.Message + Environment.NewLine + ex.StackTrace);
			result = null;
		}
		return result;
	}

	// Token: 0x06003098 RID: 12440 RVA: 0x0014C168 File Offset: 0x0014A368
	public EffectInvoker MakeEffect(MovableObjectNode mov)
	{
		return EffectInfo.MakeEffect(this, mov);
	}

	// Token: 0x04002E53 RID: 11859
	public const string EffectPrefix = "DamageInfo/";

	// Token: 0x04002E54 RID: 11860
	public DamageInfo_EffectType effectType;

	// Token: 0x04002E55 RID: 11861
	public EffectInvokedUnit invokedUnit;

	// Token: 0x04002E56 RID: 11862
	public string effectSrc = string.Empty;

	// Token: 0x04002E57 RID: 11863
	public float lifetime = 1f;

	// Token: 0x04002E58 RID: 11864
	public bool unscaled;

	// Token: 0x04002E59 RID: 11865
	public bool invokeOnce = true;

	// Token: 0x04002E5A RID: 11866
	public Vector3 relativePosition = Vector3.zero;

	// Token: 0x04002E5B RID: 11867
	public float rotation;
}
