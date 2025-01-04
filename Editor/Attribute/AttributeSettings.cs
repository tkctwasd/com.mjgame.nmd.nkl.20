#if UNITY_EDITOR

using System.Collections.Generic;

namespace MJGame {
public struct AttributeSettings
{
    public UseLocalSpaceAttribute localSpaceAttr;

    public bool useLocalSpace;
    public bool useAxisLock;

    public Axis lockedAxis;

    public AttributeSettings(IEnumerable<SettingsAttribute> attributes)
    {
        localSpaceAttr = null;

        useLocalSpace = false;
        useAxisLock = false;

        lockedAxis = Axis.X;


        foreach (var attribute in attributes)
        {
            if (attribute is UseLocalSpaceAttribute)
            {
                useLocalSpace = true;
                localSpaceAttr = (UseLocalSpaceAttribute)attribute;
            }

            if (attribute is UseAxisLockAttribute)
            {
                useAxisLock = true;
                lockedAxis = (attribute as UseAxisLockAttribute).axis;
            }
        }
    }
}
}
#endif