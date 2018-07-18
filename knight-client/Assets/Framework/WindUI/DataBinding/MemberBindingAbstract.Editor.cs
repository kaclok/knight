﻿using NaughtyAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UnityEngine.UI
{
    public partial class MemberBindingAbstract
    {
        [HideInInspector]
        public  string[]            ViewPaths   = new string[0];
        [HideInInspector]
        public  string[]            ModelPaths  = new string[0];
        
        public void GetPaths()
        {
            this.ViewPaths  = DataBindingTypeResolve.GetAllViewPaths(this.gameObject).ToArray();
            this.ViewProp   = DataBindingTypeResolve.MakeViewDataBindingProperty(this.gameObject, this.ViewPath);

            if (this.ViewProp != null)
            {
                var rViewModelProps = new List<BindableMember<PropertyInfo>>(
                    DataBindingTypeResolve.GetViewModelProperties(this.gameObject, this.ViewProp.Property.PropertyType));

                this.ModelPaths = DataBindingTypeResolve.GetAllViewModelPaths(rViewModelProps).ToArray();
            }
        }
    }
}
