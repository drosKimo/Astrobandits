using UnityEngine;

// ���������, � ������� �������� ������ � ��������� � ���� ������
public class CharacterRole : MonoBehaviour
{
    public Characters character;
    public int currentHP;
    public Roles role;
    [Tooltip("����� ��������� � �������")][Range(0, 6)] public int queueNumber;
}
