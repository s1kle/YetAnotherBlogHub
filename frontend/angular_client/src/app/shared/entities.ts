export interface blogListVm {
    blogs: blogVmForList[];
}

export interface detailedBlogVm {
    author: string;
    userId: string;
    id: string;
    title: string;
    creationDate: Date;
    details: string | null;
    editDate: Date | null;
    tags: tagVm[]
    comments: commentVm[]
}

export interface tagVm {
    id: string;
    name: string;
}

export interface commentVm {
    author: string;
    id: string;
    userId: string;
    creationDate: Date;
    content: string;
}

export interface blogVmForList {
    author: string;
    userId: string;
    id: string;
    title: string;
    creationDate: Date;
    details: string | null;
    editDate: Date | null;
    tags: tagVm[]
}

export interface searchOptions {
    searchQuery: string;
    searchProperties: string;
}

export interface sortOptions {
    sortProperty: string;
    sortDirection: string;
}

export interface createBlogVm {
    title: string;
    details?: string | null;
}

export interface createTagVm {
    name: string;
}

export interface createCommentVm {
    content: string;
}

export interface updateBlogVm {
    title: string;
    details?: string | null;
}

export interface createLinkVm {
    tagId: string;
}