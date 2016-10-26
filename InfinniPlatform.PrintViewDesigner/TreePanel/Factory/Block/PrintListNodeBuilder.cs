using InfinniPlatform.PrintView.Model;
using InfinniPlatform.PrintView.Model.Block;

namespace InfinniPlatform.PrintViewDesigner.TreePanel.Factory.Block
{
    internal class PrintListNodeBuilder : NodeBuilderBase<PrintList>
    {
        protected override void Build(NodeFactory factory, PrintElementNode parentNode, PrintList elementTemplate)
        {
            var listNode = parentNode.CreateChildNode(PrintElementNodeType.PrintListNode, elementTemplate);

            listNode.CanCut = NodeBuilderHelper.CanCut(listNode);
            listNode.Cut = NodeBuilderHelper.Cut(listNode);

            listNode.CanCopy = NodeBuilderHelper.CanCopy(listNode);
            listNode.Copy = NodeBuilderHelper.Copy(listNode);

            BuildListItemTemplate(factory, listNode, elementTemplate);
            BuildListItems(factory, listNode, elementTemplate);
        }


        private static void BuildListItemTemplate(NodeFactory factory, PrintElementNode listNode, PrintList list)
        {
            var itemTemplateNode = listNode.CreateChildNode(PrintElementNodeType.PrintListItemTemplateNode, list);

            itemTemplateNode.CanPaste = NodeBuilderHelper.CanPaste(itemTemplateNode);
            itemTemplateNode.Paste = NodeBuilderHelper.Paste(itemTemplateNode);

            itemTemplateNode.CanInsertChild = NodeBuilderHelper.CanInsertChild<PrintList, PrintBlock>(itemTemplateNode);
            itemTemplateNode.InsertChild = NodeBuilderHelper.InsertChildToContainer<PrintList, PrintBlock>(factory, itemTemplateNode, (p, c) => p.ItemTemplate = c);

            itemTemplateNode.CanDeleteChild = NodeBuilderHelper.CanDeleteChild<PrintList, PrintBlock>(itemTemplateNode);
            itemTemplateNode.DeleteChild = NodeBuilderHelper.DeleteChildFromContainer<PrintList, PrintBlock>(itemTemplateNode, (p, c) => p.ItemTemplate = null);

            factory.CreateNode(itemTemplateNode, list.ItemTemplate);
        }

        private static void BuildListItems(NodeFactory factory, PrintElementNode listNode, PrintList list)
        {
            var itemsNode = listNode.CreateChildNode(PrintElementNodeType.PrintListItemsNode, list);

            itemsNode.CanPaste = NodeBuilderHelper.CanPaste(itemsNode);
            itemsNode.Paste = NodeBuilderHelper.Paste(itemsNode);

            itemsNode.CanInsertChild = NodeBuilderHelper.CanInsertChild<PrintList, PrintBlock>(itemsNode);
            itemsNode.InsertChild = NodeBuilderHelper.InsertChildToCollection<PrintList, PrintBlock>(factory, itemsNode, p => p.Items);

            itemsNode.CanDeleteChild = NodeBuilderHelper.CanDeleteChild<PrintList, PrintBlock>(itemsNode);
            itemsNode.DeleteChild = NodeBuilderHelper.DeleteChildFromCollection<PrintList, PrintBlock>(itemsNode, p => p.Items);

            itemsNode.CanMoveChild = NodeBuilderHelper.CanMoveChildInCollection<PrintList, PrintBlock>(itemsNode);
            itemsNode.MoveChild = NodeBuilderHelper.MoveChildInCollection<PrintList, PrintBlock>(itemsNode, p => p.Items);

            factory.CreateNodes(itemsNode, list.Items);
        }
    }
}