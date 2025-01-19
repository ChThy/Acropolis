import { PropsWithChildren } from "react";

export type ElementProps = React.HTMLAttributes<HTMLElement>;
export type Props<T> = PropsWithChildren<T> &  Omit<ElementProps, keyof T>;