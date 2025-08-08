using System;

public interface IMouseCommandTargetModelExt
{
	UnitMouseEventTarget GetUnitMouseTarget();

	void SetUnitMouseTarget(UnitMouseEventTarget target);
}
