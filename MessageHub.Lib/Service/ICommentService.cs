﻿using MessageHub.Lib.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageHub.Lib.Service
{
	public interface ICommentService
	{
		int SaveComment(Comment comment);
        IEnumerable<Entity.Comment> GetComments(int id);
        IEnumerable<Entity.Comment> GetAllComments();
    }
}
