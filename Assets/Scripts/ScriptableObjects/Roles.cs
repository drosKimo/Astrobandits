using UnityEngine;

[CreateAssetMenu(fileName = "New Role", menuName = "ScriptableObjects/Role")]
public class Roles : ScriptableObject
{
    [Tooltip("���� ����������� �����")] public string roleName;
    public Sprite roleImage;
    
    [Tooltip(   "���� ��������������:\n" +
                "0 = ��� ����������\n" +
                "1 = ��������� �� ��������\n" +
                "2 = ��������� �� ����, ����� ��������")]
    [Range(0, 2)] public int rolePriority;

    [Tooltip("���� ����������, ���� ���� �������� �����?")] public bool roleEnd = false;
}
