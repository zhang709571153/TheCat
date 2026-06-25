namespace TheCat.Roguelite
{
    public readonly struct RouteNodeResolution
    {
        public RouteNodeResolution(RouteNodeResolutionType resolutionType, RouteNodeDefinition node, string message)
        {
            ResolutionType = resolutionType;
            Node = node;
            Message = message ?? string.Empty;
        }

        public RouteNodeResolutionType ResolutionType { get; }

        public RouteNodeDefinition Node { get; }

        public string Message { get; }

        public bool ShouldLoadBattle => ResolutionType == RouteNodeResolutionType.NeedsBattle;
    }
}
