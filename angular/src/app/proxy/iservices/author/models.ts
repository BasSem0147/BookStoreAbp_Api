import type { AuditedEntityDto, FullAuditedEntityDto } from '@abp/ng.core';

export interface AuthorDto extends FullAuditedEntityDto<string> {
  name?: string;
  surname?: string;
  bio?: string;
  picture?: string;
  birthDate?: string;
  deathDate?: string;
}

export interface Create_Update_Author extends AuditedEntityDto<string> {
  name?: string;
  surname?: string;
  bio?: string;
  birthDate?: string;
  deathDate?: string;
}
