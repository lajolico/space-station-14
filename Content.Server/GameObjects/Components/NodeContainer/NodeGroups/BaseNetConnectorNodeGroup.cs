﻿using Content.Server.GameObjects.Components.Power;
using Content.Server.GameObjects.Components.NodeContainer.Nodes;
using System.Collections.Generic;
using System.Linq;

namespace Content.Server.GameObjects.Components.NodeContainer.NodeGroups
{
    public abstract class BaseNetConnectorNodeGroup<TNetConnector, TNetType> : BaseNodeGroup where TNetConnector : BaseNetConnectorComponent<TNetType>
    {
        private readonly Dictionary<Node, List<TNetConnector>> _netConnectorComponents = new Dictionary<Node, List<TNetConnector>>();

        protected override void OnAddNode(Node node)
        {
            var newNetConnectorComponents = node.Owner
                .GetAllComponents<TNetConnector>()
                .Where(powerComp => (NodeGroupID) powerComp.Voltage == node.NodeGroupID)
                .ToList();
            _netConnectorComponents.Add(node, newNetConnectorComponents);
            foreach (var netConnectorComponent in newNetConnectorComponents)
            {
                SetNetConnectorNet(netConnectorComponent);
            }
        }

        protected abstract void SetNetConnectorNet(TNetConnector netConnectorComponent);

        protected override void OnRemoveNode(Node node)
        {
            foreach (var netConnectorComponent in _netConnectorComponents[node])
            {
                netConnectorComponent.ClearNet();
            }
            _netConnectorComponents.Remove(node);
        }
    }
}
