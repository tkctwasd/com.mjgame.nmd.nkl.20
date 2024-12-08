
using System;
using UnityEngine;

namespace MJGame {

[AttributeUsage(AttributeTargets.Field|AttributeTargets.Property)]
public class ReadOnlyAttribute : PropertyAttribute
{ }
}