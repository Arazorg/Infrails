using System;
using System.Collections;
using UnityEngine.Assertions;

public class UIStack
{
    public UIStack()
    {
        _stack = new Stack();
    }

    public int Count => _stack.Count;

    public void Push(IUIPanel panel)
    {
        Assert.IsNotNull(panel, "UIStack.Push: panel is null");

        // Hide top
        if (Count > 0)
        {
            IUIPanel curPanel = Peek();
            curPanel.IsActive = false;
            curPanel.OnHide();
        }

        // Push new
        _stack.Push(panel);
        panel.IsActive = true;
        panel.OnPush();
    }

    public IUIPanel Pop()
    {
        // Pop top
        IUIPanel poppedPanel = (IUIPanel)_stack.Pop();
        Assert.IsNotNull(poppedPanel, "UIStack.Pop: Top panel is null");
        poppedPanel.IsActive = false;
        poppedPanel.OnPop();

        // Show previous
        if (Count > 0)
        {
            IUIPanel newTop = Peek();
            Assert.IsNotNull(poppedPanel, "UIStack.Pop: New top panel is null");
            newTop.IsActive = true;
            newTop.OnShow();
        }

        return poppedPanel;
    }

    public void Clear()
    {
        _stack.Clear();
    }

    public IUIPanel Peek()
    {
        Assert.IsTrue(Count > 0, "UIStack.Peek: The stack is empty");
        return (IUIPanel)_stack.Peek();
    }

    public bool Contains(IUIPanel panel)
    {
        return _stack.Contains(panel);
    }

    private Stack _stack;
}