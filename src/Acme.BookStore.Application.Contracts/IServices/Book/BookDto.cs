﻿using Acme.BookStore.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Acme.BookStore.IServices.Book
{
    public class BookDto :AuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public BookType Type { get; set; }
        public DateTime PublishDate { get; set; }
        public float Price { get; set; }
        public string Picture { get; set; }
        public Guid AuthorId { get; set; }
        public string AuthorName { get; set; }
    }
}
