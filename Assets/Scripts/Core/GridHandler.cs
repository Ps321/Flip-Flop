using UnityEngine;
using UnityEngine.UI;

public class GridHandler : MonoBehaviour
{
    [SerializeField] private GridLayoutGroup grid;
    [SerializeField] private RectTransform gridRect;

    public void ConfigureGrid(int rows, int columns)
    {
        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = columns;

        float width = gridRect.rect.width;
        float height = gridRect.rect.height;

        float spacingX = grid.spacing.x;
        float spacingY = grid.spacing.y;

        float totalSpacingX = spacingX * (columns - 1);
        float totalSpacingY = spacingY * (rows - 1);

        float cellWidth = (width - totalSpacingX) / columns;
        float cellHeight = (height - totalSpacingY) / rows;

        grid.cellSize = new Vector2(cellWidth, cellHeight);
    }
}