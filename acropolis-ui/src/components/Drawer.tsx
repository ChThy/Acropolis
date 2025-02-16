import "./Drawer.css";
import { useEffect, useRef } from "react";
import { Props } from "../helpers/component-props.helper";
import cn from "classnames";
import { createPortal } from "react-dom";
import useMountTransition from "../helpers/useMountTransition";
import { FocusTrap } from "focus-trap-react";

export interface DrawerProps {
  isOpen: boolean;
  position: string;
  removeWhenClosed?: boolean;
  onClose?: () => void
}

const drawerRootId = "drawer-root";

function createPortalRoot() {
  const drawerRoot = document.createElement("div");
  drawerRoot.id = drawerRootId;

  return drawerRoot;
}

const Drawer: React.FC<Props<DrawerProps>> = ({ isOpen, className, children, position, removeWhenClosed = true, onClose }) => {
  const bodyRef = useRef(document.querySelector("body"));
  const portalRootRef = useRef(document.getElementById(drawerRootId) || createPortalRoot()); // replace || by ?? ????
  const isTransitioning = useMountTransition(isOpen, 300);

  useEffect(() => {
    const onKeyPress = (e: KeyboardEvent) => {
      if (e.key === "Escape") {
        onClose?.();
      }
    }

    if (isOpen) {
      window.addEventListener("keyup", onKeyPress);
    }

    return () => {
      window.removeEventListener("keyup", onKeyPress)
    }
  }, [isOpen, onClose])

  useEffect(() => {
    bodyRef.current?.appendChild(portalRootRef.current);

    return () => {
      portalRootRef.current.remove();
      bodyRef.current!.style.overflow = "";
    }
  }, []);

  useEffect(() => {
    const updatePageScroll = () => {
      if (isOpen) {
        bodyRef.current!.style.overflow = "hidden";
      } else {
        bodyRef.current!.style.overflow = "";
      }
    };

    updatePageScroll();
  }, [isOpen]);

  if (!isTransitioning && removeWhenClosed && !isOpen) {
    return null;
  }

  return createPortal(
    <FocusTrap>
      <div
        className={cn("drawer-container", { open: isOpen, in: isTransitioning, className })}>
        <div
          className={cn("drawer", "bg-slate-800", "text-slate-300", position)}
          role="dialog"
        >
          {children}
        </div>
        <div className="backdrop" onClick={onClose}></div>
      </div>
    </FocusTrap>,
    portalRootRef.current
  );
}

export default Drawer;