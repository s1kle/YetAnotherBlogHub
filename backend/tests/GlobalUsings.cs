global using Xunit;
global using FakeItEasy;
global using FluentAssertions;
global using FluentValidation;
global using AutoFixture;
global using AutoMapper;
global using BlogHub.Domain;
global using BlogHub.Data.Tags.Unlink;
global using BlogHub.Data.Tags.Link;
global using BlogHub.Data.Tags.Create;
global using BlogHub.Data.Tags.Delete;
global using BlogHub.Data.Tags.List.All;
global using BlogHub.Data.Tags.List.Article;
global using BlogHub.Data.Tags.List.User;
global using BlogHub.Data.Comments.Create;
global using BlogHub.Data.Comments.Delete;
global using BlogHub.Data.Comments.List.Article;
global using BlogHub.Data.Articles.Create;
global using BlogHub.Data.Articles.Delete;
global using BlogHub.Data.Articles.Update;
global using BlogHub.Data.Articles.Get;
global using BlogHub.Data.Articles.List.All;
global using BlogHub.Data.Articles.List.User;
global using BlogHub.Data.Common.Exceptions;
global using BlogHub.Data.Common.Interfaces.Articles;
global using BlogHub.Data.Common.Interfaces.Tags;
global using BlogHub.Data.Common.Interfaces.ArticleTags;
global using BlogHub.Data.Common.Interfaces.Users;
global using BlogHub.Data.Common.Interfaces.Comments;