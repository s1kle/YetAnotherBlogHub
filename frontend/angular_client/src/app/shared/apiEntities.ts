export interface Blog {
    id: string;
    title: string;
    details: string | null;
}

export interface DetailedBlog {
    id: string;
    title: string;
    creationDate: string;
    details: string | null;
    editDate: string | null;
}

export interface createBlogVM {
    title: string;
    details: string | null;
}

export interface updateBlogVM {
    title: string;
    details: string | null;
}