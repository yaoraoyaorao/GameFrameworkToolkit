using System;
using System.Collections.Generic;

namespace GameFramework.Toolkit.Runtime
{
    public delegate List<T> ReleaseObjectFilterCallback<T>(List<T> candidateObjects, int toReleaseCount, DateTime expireTime) where T : ObjectBase;
}
