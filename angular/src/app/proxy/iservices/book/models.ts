import type { AuditedEntityDto } from '@abp/ng.core';
import type { BookType } from '../../enums/book-type.enum';

export interface BookDto extends AuditedEntityDto<string> {
  name?: string;
  type?: BookType;
  publishDate?: string;
  price: number;
  picture?: string;
  authorId?: string;
  authorName?: string;
}

export interface Create_Update_Book extends AuditedEntityDto<string> {
  name?: string;
  type?: BookType;
  publishDate?: string;
  price: number;
  authorId?: string;
}
