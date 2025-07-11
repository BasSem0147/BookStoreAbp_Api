import type { PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface GetBaseList extends PagedAndSortedResultRequestDto {
  filter?: string;
}
