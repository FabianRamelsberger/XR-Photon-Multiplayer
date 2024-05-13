/* --------------------------------------------------------------------------------
# Created by: Fabian Ramelsberger
# Created Date: 2024
# --------------------------------------------------------------------------------*/

using UnityEngine;

namespace FRHelper
{
    public interface IValidatable
    {
        bool IsValid { get; }
    }
}