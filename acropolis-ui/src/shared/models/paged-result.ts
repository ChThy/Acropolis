export interface PagedResult<TResult> {
  page: number;
  pageSize: number;
  total: number;
  result: TResult[];
}

export function map<T, U, V extends PagedResult<U>>(result: V, itemMapper: (item: U) => T): PagedResult<T> {
  return {
    page: result.page,
    pageSize: result.pageSize,
    total: result.total,
    result: result.result.map<T>(itemMapper)
  };
}