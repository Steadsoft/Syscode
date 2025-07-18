﻿using System.Text;
using static SyscodeParser;

namespace Syscode
{
    public class Reference : AstNode
    {
        private Reference preceding = null; // only populated if this ref is the left of ref -> ref
        private List<Arguments> argumentsList = new();
        private BasicReference basic = null;
        private bool resolved = false;
        private Report report;
        public Reference(ReferenceContext context) : base(context)
        {
        }
        /// <summary>
        /// Indicates whether the reference has a preceding 'pointer' qualifier.
        /// </summary>
        public bool IsJustBasicReference
        {
            get { return Pointer == null; }
        }

        public bool IsResolved { get => resolved; internal set => resolved = value; }
        public bool IsntResolved { get => !resolved; }

        public bool IsntJustBasicReference { get => !IsJustBasicReference; }

        public BasicReference BasicReference { get => basic; internal set => basic = value; }
        public Reference Pointer { get => preceding; set => preceding = value; }
        public List<Arguments> ArgumentsList { get => argumentsList; set => argumentsList = value; }
        /// <summary>
        /// This is a diagnostic that must be reported if present, it is only ever present on qualified references. 
        /// </summary>
        public Report Report { get => report; set => report = value; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            if (Pointer != null)
            {
                builder.Append(Pointer.ToString());
                builder.Append(" -> ");
            }

           builder.Append(BasicReference.ToString());

           foreach (var arg in ArgumentsList)
            {
                builder.Append(arg.ToString());
            }

           return builder.ToString();
        }
    }
}