namespace Foundation.Data;

public record struct Page<TRow>(TRow[] Rows, int PageSize, int LastPosition);
