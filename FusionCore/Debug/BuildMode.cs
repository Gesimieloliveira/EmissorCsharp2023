namespace FusionCore.Debug
{
    public static class BuildMode
    {
        public static bool IsProducao
        {
            get
            {
#if DEBUG
                return false;
#else
                return true;
#endif
            }
        }

        public static bool IsHomologacao => !IsProducao;
    }
}